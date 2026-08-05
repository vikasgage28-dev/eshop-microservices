// Local dev  → each service on its own port via .env.local (localhost:5010/5011/5012/5013)
// Production → all services behind one NGINX Ingress IP via .env.production (4.187.191.129)
//              Ingress routes by path: /api/products, /api/customers, /api/orders, /api/auth
export const API_URLS = {
  catalog:  import.meta.env.VITE_API_CATALOG_URL,
  customer: import.meta.env.VITE_API_CUSTOMER_URL,
  ordering: import.meta.env.VITE_API_ORDERING_URL,
  identity: import.meta.env.VITE_API_IDENTITY_URL,
}

// ── Currency ──────────────────────────────────────────────────────
// Change locale + currency here → updates every price in the app
export const CURRENCY_LOCALE   = 'en-IN'
export const CURRENCY_CODE     = 'INR'