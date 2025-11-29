#!/usr/bin/env bash

set -euo pipefail

# Configuration
ALIAS="artnetsender"
KEYALG="RSA"
KEYSIZE="2048"
STORETYPE="PKCS12"
KEYSTORE="keystore.p12"
VALIDITY_DAYS="3650"

DEST_PATH="./src/main/resources/$KEYSTORE"

# Check for keytool
if ! command -v keytool >/dev/null 2>&1; then
    echo "keytool not found. Install a JDK or ensure keytool is in PATH."
    exit 1
fi

echo "Generating keypair..."
keytool -genkeypair \
    -alias "$ALIAS" \
    -keyalg "$KEYALG" \
    -keysize "$KEYSIZE" \
    -storetype "$STORETYPE" \
    -keystore "$KEYSTORE" \
    -validity "$VALIDITY_DAYS"

echo "Copying keystore to $DEST_PATH (overwrite enabled)..."
mkdir -p "$(dirname "$DEST_PATH")"
mv -f "$KEYSTORE" "$DEST_PATH"

echo "Done."
