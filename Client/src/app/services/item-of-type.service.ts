import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import item from '../models/itemModel';
import { catalogEndpoints } from '../constants/environment';
import { paramsKeys, pagination } from '../constants/queryParams';

@Injectable({
  providedIn: 'root',
})
export class ItemOfTypeService {
  constructor(private http: HttpClient) {}

  getItems(searchTerm?: string | undefined) {
    let params = searchTerm
      ? new HttpParams().set(paramsKeys.search, searchTerm)
      : new HttpParams();

    return this.http.get<item[]>(catalogEndpoints.items, {
      observe: 'response',
      headers: { Accept: 'application/json' },
      params: params,
    });
  }

  getItemsOfType(typeId: number) {
    return this.http.get<item[]>(`${catalogEndpoints.types}/${typeId}/items`, {
      observe: 'response',
      headers: { Accept: 'application/json' },
    });
  }

  getItemsOfTypeById(typeId: number, itemId: number) {
    return this.http.get<item>(
      `${catalogEndpoints.types}/${typeId}/items/${itemId}`
    );
  }
}
