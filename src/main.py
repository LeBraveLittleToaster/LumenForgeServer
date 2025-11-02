import math
import ssl
import time
import tkinter as tk
from tkinter import ttk, messagebox

import asyncio
import threading
from typing import Optional

import urllib3
from pyartnet import ArtNetNode
import requests
# from pyartnet.base import channel   # not needed explicitly here

# ----------------------------
# Config you can tweak
# ----------------------------
ARTNET_IP = "192.168.178.99"
ARTNET_PORT = 6454
NUM_LEDS = 1
FPS = 30
FADE_MS = 50
FRAME_SLEEP = 1 / FPS

# IMPORTANT: WLED TURN ON: >>MAIN SEGMENTS ONLY<<
# (colors list not used by your original loop; leaving here for reference)
colors = [
    [255, 0, 0],
    [0, 255, 0],
    [0, 0, 255],
]


class ArtNetWorker:
    """Runs the async Art-Net loop on its own event loop in a thread."""
    def __init__(self, on_status=None):
        self._loop: Optional[asyncio.AbstractEventLoop] = None
        self._thread: Optional[threading.Thread] = None
        self._task: Optional[asyncio.Task] = None
        self._stop_event = threading.Event()
        self._on_status = on_status or (lambda s: None)

    def start(self, dmx_address: int, dmx_universe: int):
        if self.is_running:
            self._on_status("Already running.")
            return
        self._stop_event.clear()
        self._thread = threading.Thread(
            target=self._run_loop_thread,
            args=(dmx_address, dmx_universe),
            daemon=True,
        )
        self._thread.start()

    def stop(self):
        if not self.is_running:
            self._on_status("Not running.")
            return
        self._stop_event.set()
        if self._loop and self._task:
            def _cancel():
                if not self._task.done():
                    self._task.cancel()
            self._loop.call_soon_threadsafe(_cancel)
            self._loop.call_soon_threadsafe(self._loop.stop)
        if self._thread:
            self._thread.join(timeout=2.0)
        self._loop = None
        self._task = None
        self._thread = None
        self._on_status("Stopped.")

    @property
    def is_running(self) -> bool:
        return self._thread is not None and self._thread.is_alive()

    def _run_loop_thread(self, dmx_address: int, dmx_universe: int):
        self._loop = asyncio.new_event_loop()
        asyncio.set_event_loop(self._loop)
        self._task = self._loop.create_task(
            self._sender_task(dmx_address, dmx_universe)
        )
        try:
            self._loop.run_forever()
        finally:
            # Make sure pending tasks are cleaned up
            pending = asyncio.all_tasks(loop=self._loop)
            for t in pending:
                t.cancel()
            try:
                self._loop.run_until_complete(asyncio.gather(*pending, return_exceptions=True))
            finally:
                self._loop.close()

    async def _sender_task(self, dmx_address: int, dmx_universe: int):
        try:
            self._on_status("Initializing node...")
            node = ArtNetNode(ARTNET_IP, ARTNET_PORT)

            self._on_status(f"Adding universe {dmx_universe}...")
            universe = node.add_universe(dmx_universe)

            self._on_status(f"Creating channel at address {dmx_address}...")
            channel = universe.add_channel(start=dmx_address, width=(4 * NUM_LEDS))

            # Run indefinitely until stop() called
            count = 0
            freq = 1.0
            t = 0.0
            while not self._stop_event.is_set():
                # Build payload like your original: [100, 255, 255, 0] per LED
                aggr = []
                for _ in range(NUM_LEDS):
                    phase = math.pi * 2
                    mod = (math.sin(2 * math.pi * freq * t + phase) + 1) / 2  # range 0–1
                    value = int(mod * 255)
                    aggr.extend([value, 255, 255, 0])
                    print("Sending value=" + str(value))

                # Send with fade and wait until the fade has been applied
                channel.add_fade(aggr, FADE_MS)
                await channel

                # Throttle
                await asyncio.sleep(FRAME_SLEEP)
                t = time.time()
                count += 1
                if count % FPS == 0:
                    self._on_status(f"Running... frames sent ~{count}")

        except asyncio.CancelledError:
            self._on_status("Task cancelled.")
            raise
        except Exception as e:
            self._on_status(f"Error: {e!r}")
            # Surface error but don't re-raise to avoid noisy logs
        finally:
            self._on_status("Sender task finished.")


class App(tk.Tk):
    def __init__(self):
        super().__init__()
        self.title("Art-Net Sender (Tkinter)")
        self.geometry("520x260")
        self.resizable(False, False)

        # ----------------------------
        # State (StringVars for entries)
        # ----------------------------
        self.dmx_address = tk.StringVar(value="1")
        self.dmx_universe = tk.StringVar(value="0")
        self.dmx_input_type = tk.StringVar(value="0")  # numeric field as requested
        self.dmx_mode = tk.StringVar(value="0")        # numeric field as requested

        # Validation: integers only (allow empty during typing)
        v_int = (self.register(self._is_int_or_empty), "%P")

        # ----------------------------
        # Layout
        # ----------------------------
        pad = 10
        root = ttk.Frame(self, padding=pad)
        root.pack(fill="both", expand=True)

        # Inputs frame
        inputs = ttk.LabelFrame(root, text="DMX Settings (numbers only)")
        inputs.grid(row=0, column=0, sticky="nsew", padx=0, pady=(0, pad))

        labels = ["dmxAddress", "dmxUniverse", "dmxInputType", "dmxMode"]
        vars_ = [self.dmx_address, self.dmx_universe, self.dmx_input_type, self.dmx_mode]

        for i, (label, var) in enumerate(zip(labels, vars_)):
            ttk.Label(inputs, text=label + ":").grid(row=i, column=0, sticky="e", padx=(pad, 6), pady=4)
            ttk.Entry(
                inputs,
                textvariable=var,
                width=18,
                validate="key",
                validatecommand=v_int,
            ).grid(row=i, column=1, sticky="w", padx=(0, pad), pady=4)
            ttk.Button(inputs, text="Send", command=lambda i=i, var=var: self.on_send(i, var.get())).grid(row=i, column=2)

        # Buttons
        btns = ttk.Frame(root)
        btns.grid(row=1, column=0, sticky="ew")
        ttk.Button(btns, text="Start", command=self.start_sender).grid(row=0, column=0, padx=(0, 6))
        ttk.Button(btns, text="Stop", command=self.stop_sender).grid(row=0, column=1, padx=(0, 6))
        ttk.Button(btns, text="Quit", command=self.on_quit).grid(row=0, column=2)

        # Status
        self.status = tk.StringVar(value="Idle.")
        ttk.Label(root, textvariable=self.status, anchor="w").grid(row=2, column=0, sticky="ew", pady=(pad, 0))

        # Grid weights
        root.columnconfigure(0, weight=1)

        # Worker
        self.worker = ArtNetWorker(on_status=self._set_status)

    # ----------------------------
    # UI handlers
    # ----------------------------
    def start_sender(self):
        try:
            addr = self._require_int(self.dmx_address.get().strip(), name="dmxAddress", min_val=1, max_val=512)
            uni = self._require_int(self.dmx_universe.get().strip(), name="dmxUniverse", min_val=0, max_val=32767)
        except ValueError as e:
            messagebox.showerror("Invalid input", str(e))
            return

        # Note: dmxInputType and dmxMode are present per your request,
        # but not used for sending (as specified).
        self.worker.start(addr, uni)

    def stop_sender(self):
        self.worker.stop()

    def on_quit(self):
        try:
            self.stop_sender()
        finally:
            self.destroy()

    def on_send(self, row, value):
        params = "?"
        if row == 0:
            params +=("dmxAddress=" + value + "&")
        if row == 1:
            params +=("dmxUniverse=" + value + "&")
        if row == 2:
            params += ("dmxInputType=" + value + "&")
        if row == 3:
            params += ("dmxMode=" + value + "&")
        params = params[:-1]
        try:
            session = requests.Session()
            session.auth = ("admin", "secret")
            url = f"https://192.168.178.99/internal" + params
            print(f"URL {url}")
            contents = session.get(url, verify=False)
            print(f"Row {row}: {contents}")
        finally:
            print(f"Sent request for row {row} with value {value}")

    # ----------------------------
    # Helpers
    # ----------------------------
    def _is_int_or_empty(self, proposed: str) -> bool:
        if proposed == "":
            return True
        return proposed.isdigit()

    def _require_int(self, s: str, name: str, min_val: int = None, max_val: int = None) -> int:
        if s == "":
            raise ValueError(f"{name} is required.")
        if not s.isdigit():
            raise ValueError(f"{name} must be an integer.")
        val = int(s)
        if min_val is not None and val < min_val:
            raise ValueError(f"{name} must be >= {min_val}.")
        if max_val is not None and val > max_val:
            raise ValueError(f"{name} must be <= {max_val}.")
        return val

    def _set_status(self, text: str):
        # Post updates back to main thread safely
        def _update():
            self.status.set(text)
        self.after(0, _update)


if __name__ == "__main__":
    App().mainloop()
