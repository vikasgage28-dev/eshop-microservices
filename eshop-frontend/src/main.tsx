import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { Provider } from 'react-redux'
import { Auth0Provider } from '@auth0/auth0-react'
import { store } from './app/store'
import './index.css'
import App from './App.tsx'

const domain = import.meta.env.VITE_AUTH0_DOMAIN
const clientId = import.meta.env.VITE_AUTH0_CLIENT_ID
const callbackUrl = import.meta.env.VITE_AUTH0_CALLBACK_URL

// Auth0 is only enabled when all three vars are present (i.e. local dev with .env.local).
// In production (Azure SWA) these vars are not set — social login is not yet configured
// (needs a real callback URL registered in Auth0 dashboard, coming in Stage 10/11).
// Our own JWT login (Identity.API) works in all environments regardless.
const auth0Enabled = !!(domain && clientId && callbackUrl)

const app = (
  <Provider store={store}>
    <App />
  </Provider>
)

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    {auth0Enabled ? (
      <Auth0Provider
        domain={domain!}
        clientId={clientId!}
        authorizationParams={{
          redirect_uri: callbackUrl,
          scope: 'openid profile email',
        }}
      >
        {app}
      </Auth0Provider>
    ) : (
      app
    )}
  </StrictMode>,
)