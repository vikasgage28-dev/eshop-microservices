import { useState } from 'react'
import { useParams, useNavigate } from 'react-router-dom'
import { ArrowLeft, ShoppingCart, Star } from 'lucide-react'
import { useGetProductByIdQuery, useGetReviewsByProductQuery } from '@/api/catalogApi'
import { useCart } from '@/hooks/useCart'
import { Button } from '@/components/ui/button'
import { formatCurrency } from '@/lib/utils'

export default function ProductDetailPage() {
  const { id } = useParams<{ id: string }>()
  const navigate = useNavigate()
  const { addToCart } = useCart()
  const [qty, setQty] = useState(1)

  const { data: product, isLoading } = useGetProductByIdQuery(id!)
  const { data: reviews } = useGetReviewsByProductQuery(id!)

  if (isLoading) {
    return (
      <div className="space-y-4 animate-pulse">
        <div className="h-6 bg-gray-100 rounded w-32" />
        <div className="grid grid-cols-1 md:grid-cols-2 gap-8">
          <div className="h-72 bg-gray-100 rounded-xl" />
          <div className="space-y-4">
            <div className="h-8 bg-gray-100 rounded w-3/4" />
            <div className="h-4 bg-gray-100 rounded w-1/2" />
            <div className="h-16 bg-gray-100 rounded" />
          </div>
        </div>
      </div>
    )
  }

  if (!product) {
    return <div className="text-center py-16 text-gray-400">Product not found</div>
  }

  const avgRating = reviews?.length
    ? reviews.reduce((s, r) => s + r.rating, 0) / reviews.length
    : 0

  return (
    <div className="space-y-5 max-w-4xl">
      <button
        onClick={() => navigate(-1)}
        className="flex items-center gap-1.5 text-sm text-gray-500 dark:text-gray-400 hover:text-blue-600 dark:hover:text-blue-400 transition-colors"
      >
        <ArrowLeft size={14} /> Back to products
      </button>

      <div className="bg-white dark:bg-[#2a2a2a] rounded-lg border border-[#e8e8e8] dark:border-[#3a3a3a] overflow-hidden">
        <div className="grid grid-cols-1 md:grid-cols-2">
          {/* Image */}
          <div className="h-64 md:h-auto overflow-hidden">
            <img
              src={`https://picsum.photos/seed/${product.id}/600/400`}
              alt={product.name}
              className="w-full h-full object-cover"
            />
          </div>

          {/* Info */}
          <div className="p-6 space-y-4">
            <div>
              <span className="text-xs font-medium text-blue-600 bg-blue-50 dark:bg-blue-900/30 dark:text-blue-400 px-2 py-0.5 rounded">
                {product.categoryName}
              </span>
              <h2 className="text-lg font-semibold text-gray-900 dark:text-gray-100 mt-2 leading-snug">{product.name}</h2>
            </div>

            <p className="text-gray-600 dark:text-gray-400 text-sm leading-relaxed">{product.description}</p>

            {reviews && reviews.length > 0 && (
              <div className="flex items-center gap-1.5">
                {[1,2,3,4,5].map((s) => (
                  <Star key={s} size={14} className={s <= Math.round(avgRating) ? 'fill-yellow-400 text-yellow-400' : 'text-gray-200 dark:text-gray-600'} />
                ))}
                <span className="text-xs text-gray-500 dark:text-gray-400">({reviews.length} reviews)</span>
              </div>
            )}

            <div className="flex items-baseline gap-2">
              <span className="text-2xl font-bold text-blue-600 dark:text-blue-400">{formatCurrency(product.price)}</span>
              <span className={`text-xs font-medium ${product.stock > 0 ? 'text-green-600 dark:text-green-400' : 'text-red-500'}`}>
                {product.stock > 0 ? `${product.stock} in stock` : 'Out of stock'}
              </span>
            </div>

            <div className="flex items-center gap-3 pt-1">
              <div className="flex items-center border border-[#e8e8e8] dark:border-[#444] rounded">
                <button onClick={() => setQty((q) => Math.max(1, q - 1))} className="px-3 py-1.5 text-gray-600 dark:text-gray-400 hover:bg-gray-50 dark:hover:bg-[#333] rounded-l text-base">−</button>
                <span className="px-4 py-1.5 text-sm font-semibold text-gray-900 dark:text-gray-100 border-x border-[#e8e8e8] dark:border-[#444]">{qty}</span>
                <button onClick={() => setQty((q) => Math.min(product.stock, q + 1))} className="px-3 py-1.5 text-gray-600 dark:text-gray-400 hover:bg-gray-50 dark:hover:bg-[#333] rounded-r text-base">+</button>
              </div>
              <Button
                disabled={product.stock === 0}
                onClick={() => addToCart({ productId: product.id, productName: product.name, price: product.price, quantity: qty })}
                className="flex-1 bg-blue-600 hover:bg-blue-700 h-8 text-sm"
              >
                <ShoppingCart size={14} className="mr-1.5" /> Add to Cart
              </Button>
            </div>
          </div>
        </div>
      </div>

      {/* Reviews */}
      {reviews && reviews.length > 0 && (
        <div className="bg-white dark:bg-[#2a2a2a] rounded-lg border border-[#e8e8e8] dark:border-[#3a3a3a]">
          <div className="px-4 py-3 border-b border-[#e8e8e8] dark:border-[#3a3a3a]">
            <h2 className="text-sm font-semibold text-gray-900 dark:text-gray-100">Customer Reviews</h2>
          </div>
          <div className="divide-y divide-[#f0f0f0] dark:divide-[#333]">
            {reviews.map((review) => (
              <div key={review.id} className="px-4 py-3">
                <div className="flex items-start justify-between">
                  <div>
                    <p className="text-sm font-medium text-gray-800 dark:text-gray-200">{review.userEmail}</p>
                    <div className="flex mt-1 gap-0.5">
                      {[1,2,3,4,5].map((s) => (
                        <Star key={s} size={12} className={s <= review.rating ? 'fill-yellow-400 text-yellow-400' : 'text-gray-200 dark:text-gray-600'} />
                      ))}
                    </div>
                  </div>
                  {review.verifiedPurchase && (
                    <span className="text-xs text-green-600 dark:text-green-400 bg-green-50 dark:bg-green-900/20 px-2 py-0.5 rounded">Verified</span>
                  )}
                </div>
                <p className="text-sm text-gray-600 dark:text-gray-400 mt-1.5">{review.comment}</p>
              </div>
            ))}
          </div>
        </div>
      )}
    </div>
  )
}
