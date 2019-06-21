import { Paging } from './paging';

export class PagedResult<T> {
    paging: Paging;
    data: T[];
}