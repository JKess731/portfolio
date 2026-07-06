export default function Divider({ className = '', color = 'bg-purple-500/40', width = 'w-200' }) {
  return <div className={`mx-auto h-0.5 ${width} ${color} ${className}`} />
}
