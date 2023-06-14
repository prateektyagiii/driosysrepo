import { Pipe, PipeTransform } from '@angular/core';
import { UtcToLocalTimeFormat } from './event.enum';

@Pipe({
  name: 'custom'
})

export class CustomPipe implements PipeTransform {
  
        transform(
                  utcDate: string,    
                  format: UtcToLocalTimeFormat | string 
  ): string {

      var browserLanguage = navigator.language; 
    debugger;
      if (format === UtcToLocalTimeFormat.SHORT) {
          let date = new Date(utcDate).toLocaleDateString(browserLanguage);
          let time = new Date(utcDate).toLocaleTimeString(browserLanguage);

          return `${date}, ${time}`;
      } 
    
      else if (format === UtcToLocalTimeFormat.SHORT_DATE) {
          return new Date(utcDate).toLocaleDateString(browserLanguage);
      } 
      else if (format === UtcToLocalTimeFormat.SHORT_TIME) {
          return new Date(utcDate).toLocaleTimeString(browserLanguage);
      } 
      else if (format === UtcToLocalTimeFormat.FULL) {
          return new Date(utcDate).toString();
      } 
      else {
          console.error(
              `Do not have logic to format utc date, format:${format}`
          );
          return new Date(utcDate).toString();
      }
  }

  public convertLocalTimeToUtc(localDate: string):string{
      var date = new Date(localDate);
      return date.toUTCString();
  }
}
