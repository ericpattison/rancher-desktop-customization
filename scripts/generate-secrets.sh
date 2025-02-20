#!/usr/bin/env bash

set -euo pipefail

check_dependencies() {
  local dependencies=(kubectl kubeseal openssl base64)
  for cmd in "${dependencies[@]}"; do
    if ! command -v "$cmd" &>/dev/null; then
      echo "Error: '$cmd' command not found. Please install it before running this script."
      exit 1
    fi
  done
}

generate_password() {
  # Generates a 16-character alphanumeric password
  openssl rand -base64 12 | tr -dc 'a-zA-Z0-9' | head -c 16
}

create_sealed_secret() {
  local secret_name="$1"
  local namespace="$2"
  local output_dir="$3"
  shift 3
  local literals=("$@")
  local output_file="$output_dir/${secret_name}.yaml"

  echo "Creating and sealing secret: $secret_name"

  # Create the Kubernetes Secret and seal it
  kubectl create secret generic "$secret_name" "${literals[@]}" \
    --namespace "$namespace" --dry-run=client -o yaml | \
  kubeseal --controller-name="sealed-secrets" --controller-namespace="sealed-secrets" -o yaml > "$output_file"

  echo "SealedSecret saved to: $output_file"
}

check_dependencies

#kubectl create ns keycloak-system
mkdir -p .secrets

create_sealed_secret \
  "keycloak-admin-password" \
  "keycloak" \
  ".secrets" \
  "--from-literal=password=password"

create_sealed_secret \
  "postgresql-secret" \
  "keycloak" \
  ".secrets" \
  "--from-literal=password=$(generate_password)" \
  "--from-literal=postgres-password=$(generate_password)" \
  "--from-literal=repmgr-password=$(generate_password)"

create_sealed_secret \
  "pgpool-secret" \
  "keycloak" \
  ".secrets" \
  "--from-literal=admin-password=$(generate_password)"

kubectl apply -f ./.secrets -n keycloak