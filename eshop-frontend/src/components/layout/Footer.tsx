const APP_VERSION = import.meta.env.VITE_APP_VERSION ?? '0.0.0-dev'
const BUILD_YEAR = new Date().getFullYear()

export default function Footer() {
  return (
    <footer className="h-8 bg-white dark:bg-[#1e1e1e] border-t border-[#e8e8e8] dark:border-[#2d2d2d] flex items-center justify-between px-5 flex-shrink-0 text-[0.7rem] text-gray-400 dark:text-gray-500">
      <span>© {BUILD_YEAR} eShop Microservices</span>
      <span title="Build version" className="font-mono">
        v{APP_VERSION}
      </span>
    </footer>
  )
}