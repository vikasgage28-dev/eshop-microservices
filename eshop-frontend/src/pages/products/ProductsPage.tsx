import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { Search, Plus, ShoppingCart } from 'lucide-react'
import { useGetProductsQuery, useGetCategoriesQuery } from '@/api/catalogApi'
import { useCart } from '@/hooks/useCart'
import { useAuth } from '@/hooks/useAuth'
import { useDebounce } from '@/hooks/useDebounce'
import { Button } from '@/components/ui/button'
import { formatCurrency } from '@/lib/utils'

export default function ProductsPage() {
  const navigate = useNavigate()
  const { addToCart } = useCart()
  const { isAdmin } = useAuth()

  const [search, setSearch] = useState('')
  const [categoryId, setCategoryId] = useState('')
  const [page, setPage] = useState(1)
  const pageSize = 12

  const debouncedSearch = useDebounce(search, 400)

  const { data, isLoading, isFetching } = useGetProductsQuery({
    page, pageSize, search: debouncedSearch || undefined, categoryId: categoryId || undefined,
  })
  const { data: categories } = useGetCategoriesQuery()

  const totalPages = data ? Math.ceil(data.totalCount / pageSize) : 0

  return (
    <div className="space-y-5">
      <div className="flex items-center justify-between">
        <p className="text-sm text-gray-500">{data?.totalCount ?? 0} products</p>
        {isAdmin && (
          <Button className="bg-blue-600 hover:bg-blue-700 h-8 text-sm px-3" onClick={() => navigate('/admin')}>
            <Plus size={14} className="mr-1" /> Add Product
          </Button>
        )}
      </div>

      {/* Filters */}
      <div className="flex gap-3 flex-wrap">
        <div className="relative flex-1 min-w-48">
          <Search size={16} className="absolute left-3 top-1/2 -translate-y-1/2 text-gray-400" />
          <input
            value={search}
            onChange={(e) => { setSearch(e.target.value); setPage(1) }}
            placeholder="Search products…"
            className="w-full pl-9 pr-4 py-2 border border-gray-200 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
          />
        </div>
        <select
          value={categoryId}
          onChange={(e) => { setCategoryId(e.target.value); setPage(1) }}
          className="px-3 py-2 border border-gray-200 rounded-lg text-sm bg-white focus:outline-none focus:ring-2 focus:ring-blue-500"
        >
          <option value="">All categories</option>
          {categories?.map((c) => (
            <option key={c.id} value={c.id}>{c.name}</option>
          ))}
        </select>
      </div>

      {/* Grid */}
      {isLoading || isFetching ? (
        <div className="grid grid-cols-2 sm:grid-cols-3 lg:grid-cols-4 gap-4">
          {Array.from({ length: 8 }).map((_, i) => (
            <div key={i} className="bg-white dark:bg-[#2a2a2a] rounded-lg border border-[#e8e8e8] dark:border-[#3a3a3a] p-4 animate-pulse">
              <div className="h-28 bg-gray-100 dark:bg-[#333] rounded mb-3" />
              <div className="h-3.5 bg-gray-100 dark:bg-[#333] rounded w-3/4 mb-2" />
              <div className="h-3 bg-gray-100 dark:bg-[#333] rounded w-1/2" />
            </div>
          ))}
        </div>
      ) : (
        <div className="grid grid-cols-2 sm:grid-cols-3 lg:grid-cols-4 gap-4">
          {data?.items.map((product) => (
            <div
              key={product.id}
              className="bg-white dark:bg-[#2a2a2a] rounded-lg border border-[#e8e8e8] dark:border-[#3a3a3a] p-3 flex flex-col gap-2.5 hover:border-blue-300 dark:hover:border-blue-600 hover:bg-blue-50/20 dark:hover:bg-blue-900/10 transition-colors cursor-pointer"
              onClick={() => navigate(`/products/${product.id}`)}
            >
              <div className="h-28 rounded overflow-hidden">
                <img
                  src={`https://picsum.photos/seed/${product.id}/300/200`}
                  alt={product.name}
                  className="w-full h-full object-cover"
                  loading="lazy"
                />
              </div>
              <div className="flex-1">
                <p className="text-sm font-medium text-gray-900 dark:text-gray-100 line-clamp-2 leading-snug">{product.name}</p>
                <p className="text-xs text-gray-400 dark:text-gray-500 mt-0.5">{product.categoryName}</p>
              </div>
              <div className="flex items-center justify-between">
                <span className="text-sm font-semibold text-blue-600">{formatCurrency(product.price)}</span>
                <button
                  onClick={(e) => {
                    e.stopPropagation()
                    addToCart({ productId: product.id, productName: product.name, price: product.price, quantity: 1 })
                  }}
                  className="p-1.5 rounded bg-blue-600 text-white hover:bg-blue-700 transition-colors"
                >
                  <ShoppingCart size={13} />
                </button>
              </div>
            </div>
          ))}
          {data?.items.length === 0 && (
            <div className="col-span-full text-center py-16 text-gray-400">No products found</div>
          )}
        </div>
      )}

      {/* Pagination */}
      {totalPages > 1 && (
        <div className="flex items-center justify-center gap-2">
          <button
            disabled={page === 1}
            onClick={() => setPage((p) => p - 1)}
            className="px-3 py-1.5 text-sm border border-gray-200 rounded-lg disabled:opacity-40 hover:bg-gray-50"
          >
            Previous
          </button>
          <span className="text-sm text-gray-600">{page} / {totalPages}</span>
          <button
            disabled={page === totalPages}
            onClick={() => setPage((p) => p + 1)}
            className="px-3 py-1.5 text-sm border border-gray-200 rounded-lg disabled:opacity-40 hover:bg-gray-50"
          >
            Next
          </button>
        </div>
      )}
    </div>
  )
}
