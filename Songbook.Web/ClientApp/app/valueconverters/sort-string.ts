export class SortStringValueConverter {
  public static SortAscending: string = 'ASCENDING';
  public static SortDescending: string = 'DESCENDING';

  public toView(
      array: any[],
      propertyName: string,
      direction: string): any[] {

    var factor = (direction === SortStringValueConverter.SortAscending) ? 1 : -1;

    return array
      .slice(0)
      .sort((a, b) => {
        return SortStringValueConverter.stringComparisonOrdinalIgnoreCase(
          a[propertyName],
          b[propertyName]) * factor;
      });
  }

  public static stringComparisonOrdinalIgnoreCase(a: string, b: string): number {
    if (a === null || typeof a === 'undefined')
      a = '';
    if (b === null || typeof b === 'undefined')
      b = '';

    a = a.toLowerCase();
    b = b.toLowerCase();

    return (a < b) ? -1 :
           (a > b) ? 1 : 
           0;
  }
}