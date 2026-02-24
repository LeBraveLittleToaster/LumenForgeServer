#!/bin/bash

# --- Configuration ---
KEYCLOAK_URL="http://localhost:8080"
ADMIN_REALM="master"
TARGET_REALM="lumenforge-realm"
ADMIN_USER="admin"
ADMIN_PASS="adminpassword"

# New User Details
NEW_USERNAME="charlie"
NEW_EMAIL="charlie@example.com"
NEW_PASSWORD="charlie123"

# --- Check Dependencies ---
if ! command -v jq &> /dev/null; then
    echo "Error: 'jq' is not installed. Please install it to parse JSON responses."
    exit 1
fi

echo "Step 1: Authenticating as $ADMIN_USER..."

# Get Admin Access Token
RESPONSE=$(curl -s -X POST "$KEYCLOAK_URL/realms/$ADMIN_REALM/protocol/openid-connect/token" \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "username=$ADMIN_USER" \
  -d "password=$ADMIN_PASS" \
  -d "grant_type=password" \
  -d "client_id=admin-cli")

ACCESS_TOKEN=$(echo $RESPONSE | jq -r '.access_token')

if [ "$ACCESS_TOKEN" == "null" ] || [ -z "$ACCESS_TOKEN" ]; then
    echo "Error: Failed to obtain access token. Check your credentials and URL."
    echo "Response: $RESPONSE"
    exit 1
fi

echo "Step 2: Creating user '$NEW_USERNAME' in realm '$TARGET_REALM'..."

# Create User Request
# Note: We capture the HTTP status code to verify success
HTTP_STATUS=$(curl -s -o /dev/null -w "%{http_code}" -X POST "$KEYCLOAK_URL/admin/realms/$TARGET_REALM/users" \
  -H "Authorization: Bearer $ACCESS_TOKEN" \
  -H "Content-Type: application/json" \
  -d "{
    \"username\": \"$NEW_USERNAME\",
    \"enabled\": true,
    \"firstName\": \"Charlie\",
    \"lastName\": \"Worker\",
    \"email\": \"$NEW_EMAIL\",
    \"emailVerified\": true,
    \"groups\": [\"users\"],
    \"credentials\": [
      {
        \"type\": \"password\",
        \"value\": \"$NEW_PASSWORD\",
        \"temporary\": false
      }
    ]
  }")

# --- Feedback ---
if [ "$HTTP_STATUS" -eq 201 ]; then
    echo "Successfully created user: $NEW_USERNAME"
elif [ "$HTTP_STATUS" -eq 409 ]; then
    echo "Error: User '$NEW_USERNAME' or email '$NEW_EMAIL' already exists (409 Conflict)."
else
    echo "Failed to create user. Received HTTP status: $HTTP_STATUS"
fi