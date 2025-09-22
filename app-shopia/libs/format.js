export function formatCurrency(num) {
  return '$ ' + Intl.NumberFormat('es-ES', {
    style: 'decimal',
    minimumFractionDigits: 2,
    maximumFractionDigits: 2
  }).format(num);
}

export function toFixed(num, fixed) {
  return Number.parseFloat(num).toFixed(fixed).replaceAll(/\./g, ',');
}