import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import item from '../models/itemModel';
import { endpoints } from '../constants/environment';

@Injectable({
  providedIn: 'root',
})
export class ItemOfTypeService {
  constructor(private http: HttpClient) {}

  getItemsOfType(typeId: number) {
    return this.http.get<item[]>(`${endpoints.types}/${typeId}/items`, {
      observe: 'response',
      headers: { Accept: 'application/json' },
    });
  }

  getItemsOfTypeById(typeId: number, itemId: number) {
    return this.http.get<item>(`${endpoints.types}/${typeId}/items/${itemId}`);
  }
}
