import { formatDate } from "@viasoft/components/shared";

export function formatLocalizedDate(dateString: string): string {
  const date = getLocalizedDate(dateString)
  return formatDate(date);
};

export function getLocalizedDate(dateString: string | Date): Date {
  if (typeof dateString !== 'string') {
    return new Date(dateString);
  }
  const gmtOffset = -new Date().getTimezoneOffset() / 60;
  const date = new Date(dateString);
  date.setHours(date.getHours() + (-1 * gmtOffset));
  return date;
};