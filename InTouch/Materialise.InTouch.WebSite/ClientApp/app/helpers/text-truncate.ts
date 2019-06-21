import { Pipe } from '@angular/core'

@Pipe({
    name: 'textTruncate'
})
export class TruncatePipe {
    transform(value: string, truncateLength: number, trail: string): string {

        return value.length > truncateLength ? value.substring(0, truncateLength).trim() + trail : value;
    }
}