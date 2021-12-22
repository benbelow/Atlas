// This file is for outputs that are read by other terraform scripts as a remote state.
// If you want an output to be read by azure, use CIOutputs.tf

output "donor_import" {
  value = module.donor_import
}

output "match_prediction" {
  value = module.match_prediction
}

output "matching_algorithm" {
  value = module.matching_algorithm
}

output "multiple_allele_code_lookup" {
  value = module.multiple_allele_code_lookup
}

output "public_api_function" {
  value = {
    api_key = var.PUBLIC_API_FUNCTION_HOST_KEY
    base_url = "https://${azurerm_function_app.atlas_public_api_function.default_site_hostname}"
  }
}

output "sql_server" {
  value = azurerm_sql_server.atlas_sql_server.name
}

output "storage_account" {
  value = {
    id = azurerm_storage_account.azure_storage.id
  }
}
