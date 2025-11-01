import tkinter as tk
from tkinter import ttk
from tkinter import messagebox


class App(tk.Tk):
    def __init__(self):
        super().__init__()
        self.title("Simple Modes & Inputs")
        self.geometry("420x280")
        self.resizable(False, False)

        # ---------- State ----------
        self.mode = tk.StringVar(value="Mode A")
        self.inputs = [tk.StringVar(value="") for _ in range(4)]

        # ---------- Validation ----------
        # Allow empty (so user can type) or a valid float/int
        vcmd = (self.register(self._is_number_or_empty), "%P")

        # ---------- Layout ----------
        content = ttk.Frame(self, padding=12)
        content.pack(fill="both", expand=True)

        # Modes
        modes_frame = ttk.LabelFrame(content, text="Mode")
        modes_frame.grid(row=0, column=0, sticky="nsew", padx=(0, 12))
        for i, label in enumerate(("Mode A", "Mode B", "Mode C", "Mode D")):
            ttk.Radiobutton(
                modes_frame, text=label, value=label, variable=self.mode
            ).grid(row=i, column=0, sticky="w", pady=2, padx=8)

        # Inputs
        inputs_frame = ttk.LabelFrame(content, text="Inputs (numbers only)")
        inputs_frame.grid(row=0, column=1, sticky="nsew")
        for i, (sv, name) in enumerate(
            zip(self.inputs, ("Value 1", "Value 2", "Value 3", "Value 4"))
        ):
            ttk.Label(inputs_frame, text=name + ":").grid(
                row=i, column=0, sticky="e", pady=4, padx=(8, 6)
            )
            ttk.Entry(
                inputs_frame,
                textvariable=sv,
                width=16,
                validate="key",
                validatecommand=vcmd,
            ).grid(row=i, column=1, sticky="w", pady=4, padx=(0, 8))

        # Buttons
        btns = ttk.Frame(content)
        btns.grid(row=1, column=0, columnspan=2, sticky="ew", pady=(12, 0))
        btns.columnconfigure(0, weight=1)
        ttk.Button(btns, text="Apply", command=self.apply).grid(
            row=0, column=0, sticky="w"
        )
        ttk.Button(btns, text="Quit", command=self.destroy).grid(
            row=0, column=1, sticky="e"
        )

        # Grid weights
        content.columnconfigure(0, weight=1)
        content.columnconfigure(1, weight=1)

        # Styling hint (optional light padding)
        for child in content.winfo_children():
            for sub in child.winfo_children():
                if isinstance(sub, ttk.Entry):
                    sub.configure()

    # ---------- Helpers ----------
    def _is_number_or_empty(self, proposed: str) -> bool:
        if proposed == "":
            return True
        try:
            float(proposed)
            return True
        except ValueError:
            return False

    def apply(self):
        # Convert to floats where possible; warn if any empty
        values = []
        empty_fields = []
        for i, sv in enumerate(self.inputs, start=1):
            txt = sv.get().strip()
            if txt == "":
                empty_fields.append(f"Value {i}")
            else:
                values.append(float(txt))

        if empty_fields:
            messagebox.showwarning(
                "Missing input",
                "Please fill: " + ", ".join(empty_fields),
            )
            return

        summary = (
            f"Mode: {self.mode.get()}\n"
            f"Values: {', '.join(map(str, values))}"
        )
        messagebox.showinfo("Current Settings", summary)


if __name__ == "__main__":
    App().mainloop()
