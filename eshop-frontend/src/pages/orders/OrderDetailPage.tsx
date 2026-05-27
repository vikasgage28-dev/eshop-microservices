import { useParams, useNavigate } from 'react-router-dom'
import { ArrowLeft, Package, MapPin, Calendar, Hash } from 'lucide-react'
import { useGetOrderByIdQuery, useCancelOrderMutation } from '@/api/orderingApi'
import { formatCurrency } from '@/lib/utils'
import { Button } from '@/components/ui/button'

const STATUS_COLORS: Record<string, string> = {
  Pending:    'bg-yellow-100 text-yellow-700 dark:bg-yellow-900/30 dark:text-yellow-400',
  Confirmed:  'bg-blue-100 text-blue-700 dark:bg-blue-900/30 dark:text-blue-400',
  Processing: 'bg-purple-100 text-purple-700 dark:bg-purple-900/30 dark:text-purple-400',
  Shipped:    'bg-indigo-100 text-indigo-700 dark:bg-indigo-900/30 dark:text-indigo-400',
  Delivered:  'bg-green-100 text-green-700 dark:bg-green-900/30 dark:text-green-400',
  Cancelled:  'bg-red-100 text-red-700 dark:bg-red-900/30 dark:text-red-400',
}

export default function OrderDetailPage() {
  const { id } = useParams<{ id: string }>()
  const navigate = useNavigate()
  const { data: order, isLoading } = useGetOrderByIdQuery(id!)
  const [cancelOrder, { isLoading: cancelling }] = useCancelOrderMutation()

  if (isLoading) {
    return (
      <div className="max-w-2xl space-y-4 animate-pulse">
        <div className="h-5 bg-gray-100 dark:bg-[#333] rounded w-32" />
        <div className="h-40 bg-gray-100 dark:bg-[#333] rounded-lg" />
        <div className="h-48 bg-gray-100 dark:bg-[#333] rounded-lg" />
      </div>
    )
  }

  if (!order) {
    return <div className="text-center py-16 text-gray-400">Order not found</div>
  }

  const handleCancel = async () => {
    await cancelOrder({ id: order.id, reason: 'Cancelled by customer' })
    navigate('/orders')
  }

  const statusClass = STATUS_COLORS[order.status] ?? STATUS_COLORS.Pending

  return (
    <div className="max-w-2xl space-y-4">
      <button
        onClick={() => navigate('/orders')}
        className="flex items-center gap-1.5 text-sm text-gray-500 dark:text-gray-400 hover:text-blue-600 dark:hover:text-blue-400 transition-colors"
      >
        <ArrowLeft size={14} /> Back to Orders
      </button>

      {/* Header card */}
      <div className="bg-white dark:bg-[#2a2a2a] rounded-lg border border-[#e8e8e8] dark:border-[#3a3a3a] p-5">
        <div className="flex items-start justify-between gap-4">
          <div className="space-y-2">
            <div className="flex items-center gap-2">
              <Hash size={13} className="text-gray-400" />
              <span className="font-mono text-xs text-gray-500 dark:text-gray-400">{order.id}</span>
            </div>
            <div className="flex items-center gap-2">
              <Calendar size={13} className="text-gray-400" />
              <span className="text-sm text-gray-700 dark:text-gray-300">
                {new Date(order.orderDate).toLocaleDateString('en-IN', { day: 'numeric', month: 'long', year: 'numeric' })}
              </span>
            </div>
            {order.shippingAddress && (
              <div className="flex items-center gap-2">
                <MapPin size={13} className="text-gray-400" />
                <span className="text-sm text-gray-700 dark:text-gray-300">{order.shippingAddress}</span>
              </div>
            )}
          </div>
          <span className={`px-3 py-1 rounded text-xs font-semibold flex-shrink-0 ${statusClass}`}>
            {order.statusName ?? order.status}
          </span>
        </div>

        <div className="mt-4 pt-4 border-t border-[#e8e8e8] dark:border-[#3a3a3a] flex items-center justify-between">
          <span className="text-sm text-gray-500 dark:text-gray-400">Order Total</span>
          <span className="text-lg font-bold text-gray-900 dark:text-gray-100">{formatCurrency(order.totalAmount)}</span>
        </div>
      </div>

      {/* Items */}
      <div className="bg-white dark:bg-[#2a2a2a] rounded-lg border border-[#e8e8e8] dark:border-[#3a3a3a]">
        <div className="px-4 py-3 border-b border-[#e8e8e8] dark:border-[#3a3a3a]">
          <h2 className="text-sm font-semibold text-gray-900 dark:text-gray-100">
            Items ({order.items.length})
          </h2>
        </div>
        <div className="divide-y divide-[#f0f0f0] dark:divide-[#333]">
          {order.items.map((item) => (
            <div key={item.id} className="flex items-center gap-3 px-4 py-3">
              <div className="w-10 h-10 bg-[#f4f4f4] dark:bg-[#333] rounded overflow-hidden flex-shrink-0">
                <img
                  src={`https://picsum.photos/seed/${item.productId}/80/80`}
                  alt={item.productName}
                  className="w-full h-full object-cover"
                />
              </div>
              <div className="flex-1 min-w-0">
                <p className="text-sm font-medium text-gray-900 dark:text-gray-100 truncate">{item.productName}</p>
                <p className="text-xs text-gray-500 dark:text-gray-400">
                  {formatCurrency(item.unitPrice)} × {item.quantity}
                </p>
              </div>
              <span className="text-sm font-semibold text-gray-900 dark:text-gray-100 flex-shrink-0">
                {formatCurrency(item.unitPrice * item.quantity)}
              </span>
            </div>
          ))}
        </div>
      </div>

      {/* Cancel */}
      {order.status === 'Pending' && (
        <div className="flex justify-end">
          <Button
            variant="outline"
            onClick={handleCancel}
            disabled={cancelling}
            className="text-sm text-red-600 dark:text-red-400 border-red-200 dark:border-red-800 hover:bg-red-50 dark:hover:bg-red-900/20"
          >
            <Package size={14} className="mr-1.5" />
            {cancelling ? 'Cancelling…' : 'Cancel Order'}
          </Button>
        </div>
      )}
    </div>
  )
}
