import { Bell, ShoppingCart, LogOut, User, Moon, Sun } from 'lucide-react'
import { useNavigate, useLocation } from 'react-router-dom'
import { useAuth } from '@/hooks/useAuth'
import { useCart } from '@/hooks/useCart'
import { useDarkMode } from '@/hooks/useDarkMode'
import { Button } from '@/components/ui/button'

/** Map path prefix → human-readable page title */
const PAGE_TITLES: Record<string, string> = {
  '/dashboard':  'Dashboard',
  '/products':   'Products',
  '/cart':       'Cart',
  '/checkout':   'Checkout',
  '/orders':     'Orders',
  '/customers':  'Customers',
  '/admin':      'Admin',
  '/profile':    'Profile',
}

function usePageTitle(): string {
  const { pathname } = useLocation()
  // match longest prefix first so /products/123 → "Products"
  const match = Object.keys(PAGE_TITLES)
    .filter((k) => pathname === k || pathname.startsWith(k + '/'))
    .sort((a, b) => b.length - a.length)[0]
  return match ? PAGE_TITLES[match] : 'eShop'
}

export default function TopBar() {
  const { fullName, email, logout } = useAuth()
  const { totalItems } = useCart()
  const { isDark, toggle: toggleDark } = useDarkMode()
  const navigate = useNavigate()
  const pageTitle = usePageTitle()

  const handleLogout = () => { logout(); navigate('/login') }
  const initials = fullName ? fullName.split(' ').map((n) => n[0]).join('').slice(0, 2).toUpperCase() : 'U'

  return (
    <header className="h-14 bg-white dark:bg-[#1e1e1e] border-b border-[#e8e8e8] dark:border-[#2d2d2d] flex items-center justify-between px-5 flex-shrink-0">
      {/* Left — current page name */}
      <h1 className="text-base font-semibold text-gray-900 dark:text-gray-100 tracking-[-0.01em]">{pageTitle}</h1>

      {/* Right — actions */}
      <div className="flex items-center gap-1">
        {/* Dark mode toggle */}
        <button
          onClick={toggleDark}
          className="p-2 text-gray-500 dark:text-gray-400 hover:text-blue-600 dark:hover:text-blue-400 hover:bg-blue-50 dark:hover:bg-blue-900/20 rounded-lg transition-colors"
          title={isDark ? 'Switch to light mode' : 'Switch to dark mode'}
        >
          {isDark ? <Sun size={18} /> : <Moon size={18} />}
        </button>

        {/* Cart */}
        <button
          onClick={() => navigate('/cart')}
          className="relative p-2 text-gray-500 dark:text-gray-400 hover:text-blue-600 dark:hover:text-blue-400 hover:bg-blue-50 dark:hover:bg-blue-900/20 rounded-lg transition-colors"
        >
          <ShoppingCart size={18} />
          {totalItems > 0 && (
            <span className="absolute -top-0.5 -right-0.5 w-4 h-4 bg-blue-600 text-white text-[0.55rem] font-bold rounded-full flex items-center justify-center">
              {totalItems > 9 ? '9+' : totalItems}
            </span>
          )}
        </button>

        {/* Notifications */}
        <button className="p-2 text-gray-500 dark:text-gray-400 hover:text-blue-600 dark:hover:text-blue-400 hover:bg-blue-50 dark:hover:bg-blue-900/20 rounded-lg transition-colors">
          <Bell size={18} />
        </button>

        {/* User avatar — click to go to profile */}
        <button
          onClick={() => navigate('/profile')}
          className="flex items-center gap-2 pl-3 ml-1 border-l border-gray-200 dark:border-[#333] hover:opacity-80 transition-opacity"
          title="My Profile"
        >
          <div className="w-7 h-7 bg-blue-600 rounded-full flex items-center justify-center flex-shrink-0">
            <span className="text-white text-xs font-semibold">{initials}</span>
          </div>
          <div className="hidden sm:block text-left">
            <p className="text-xs font-semibold text-gray-800 dark:text-gray-200 leading-tight">{fullName ?? 'User'}</p>
            <p className="text-[0.65rem] text-gray-400 dark:text-gray-500 leading-tight">{email}</p>
          </div>
        </button>

        {/* Logout */}
        <Button
          variant="ghost"
          size="sm"
          onClick={handleLogout}
          className="text-gray-400 dark:text-gray-500 hover:text-red-600 dark:hover:text-red-400 hover:bg-red-50 dark:hover:bg-red-900/20 ml-1 px-2"
          title="Log out"
        >
          <LogOut size={16} />
        </Button>
      </div>
    </header>
  )
}
