import { useState } from 'react'
import { NavLink } from 'react-router-dom'
import {
  LayoutDashboard, Package, ShoppingCart, Users, Settings,
  ChevronLeft, ChevronRight, ShieldCheck, Store, ClipboardList, UserCircle,
} from 'lucide-react'
import { cn } from '@/lib/utils'
import { useAuth } from '@/hooks/useAuth'
import { useCart } from '@/hooks/useCart'

interface NavItem {
  to: string
  icon: React.ReactNode
  label: string
  adminOnly?: boolean
}

// adminOnly: true  → visible and accessible to Admin only
// no flag         → visible to all logged-in users
const navItems: NavItem[] = [
  { to: '/dashboard',  icon: <LayoutDashboard size={20} />, label: 'Dashboard',  adminOnly: true },
  { to: '/products',   icon: <Package size={20} />,         label: 'Products' },
  { to: '/cart',       icon: <ShoppingCart size={20} />,    label: 'Cart' },
  { to: '/orders',     icon: <ClipboardList size={20} />,   label: 'Orders' },
  { to: '/profile',    icon: <UserCircle size={20} />,      label: 'Profile' },
  { to: '/customers',  icon: <Users size={20} />,           label: 'Customers',  adminOnly: true },
  { to: '/admin',      icon: <ShieldCheck size={20} />,     label: 'Admin',      adminOnly: true },
]

export default function Sidebar() {
  const [collapsed, setCollapsed] = useState(false)
  const { isAdmin } = useAuth()
  const { totalItems } = useCart()

  return (
    <aside
      className={cn(
        'flex flex-col h-screen bg-white dark:bg-[#1e1e1e] border-r border-[#e8e8e8] dark:border-[#2d2d2d] transition-all duration-200 select-none flex-shrink-0',
        collapsed ? 'w-[52px]' : 'w-[165px]'
      )}
    >
      {/* ── Logo block — same height as TopBar (56px) ───────────────────── */}
      <div
        className={cn(
          'flex items-center h-14 border-b border-[#e8e8e8] dark:border-[#2d2d2d] flex-shrink-0',
          collapsed ? 'justify-center px-0' : 'gap-2.5 px-4'
        )}
      >
        <div className="w-7 h-7 bg-blue-600 rounded-[6px] flex items-center justify-center flex-shrink-0">
          <Store size={14} className="text-white" />
        </div>
        {!collapsed && (
          <span className="font-semibold text-sm text-gray-900 dark:text-gray-100 tracking-[-0.01em]">eShop</span>
        )}
      </div>

      {/* ── Navigation ───────────────────────────────────────────────────── */}
      <nav className="flex-1 py-2 overflow-y-auto">
        {navItems
          .filter((item) => !item.adminOnly || isAdmin)
          .map((item) => (
            <NavLink
              key={item.to}
              to={item.to}
              title={collapsed ? item.label : undefined}
              className={({ isActive }) =>
                cn(
                  /* base — Lenovo Vantage: full-width, 40px height, no gap between items */
                  'flex items-center h-10 text-sm font-[450] transition-colors duration-150',
                  collapsed ? 'justify-center px-0 mx-0' : 'gap-3 px-4',
                  isActive
                    ? 'bg-blue-600 text-white'
                    : 'text-[#444] dark:text-[#bbb] hover:bg-[#f0f0f0] dark:hover:bg-[#2a2a2a] hover:text-[#111] dark:hover:text-[#eee]'
                )
              }
            >
              {/* Icon */}
              <span className="relative flex-shrink-0">
                <span className={collapsed ? '' : '[&>svg]:w-[18px] [&>svg]:h-[18px]'}>
                  {item.icon}
                </span>
                {/* Cart badge (collapsed mode only) */}
                {item.to === '/cart' && totalItems > 0 && collapsed && (
                  <span className="absolute -top-1 -right-1 w-3.5 h-3.5 bg-red-500 text-white text-[0.5rem] font-bold rounded-full flex items-center justify-center">
                    {totalItems > 9 ? '9+' : totalItems}
                  </span>
                )}
              </span>

              {/* Label + optional cart count */}
              {!collapsed && (
                <span className="flex-1 flex items-center justify-between leading-none">
                  {item.label}
                  {item.to === '/cart' && totalItems > 0 && (
                    <span className="text-[0.6rem] font-semibold bg-white/25 px-1.5 py-0.5 rounded-full">
                      {totalItems}
                    </span>
                  )}
                </span>
              )}
            </NavLink>
          ))}
      </nav>

      {/* ── Settings ─────────────────────────────────────────────────────── */}
      <div className="border-t border-[#e8e8e8] dark:border-[#2d2d2d]">
        <button
          className={cn(
            'w-full flex items-center h-10 text-sm text-[#888] dark:text-[#666] hover:bg-[#f0f0f0] dark:hover:bg-[#2a2a2a] hover:text-[#333] dark:hover:text-[#ccc] transition-colors',
            collapsed ? 'justify-center' : 'gap-3 px-4'
          )}
          title={collapsed ? 'Settings' : undefined}
        >
          <Settings size={18} />
          {!collapsed && <span>Settings</span>}
        </button>

        {/* Collapse toggle */}
        <button
          onClick={() => setCollapsed((v) => !v)}
          className={cn(
            'w-full flex items-center h-9 text-[#aaa] dark:text-[#555] hover:text-blue-600 dark:hover:text-blue-400 hover:bg-blue-50 dark:hover:bg-blue-900/20 transition-colors border-t border-[#e8e8e8] dark:border-[#2d2d2d]',
            collapsed ? 'justify-center' : 'justify-end px-3'
          )}
        >
          {collapsed ? <ChevronRight size={14} /> : <ChevronLeft size={14} />}
        </button>
      </div>
    </aside>
  )
}
