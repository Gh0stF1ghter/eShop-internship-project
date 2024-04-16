import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import item from '../models/itemModel';

@Injectable({
  providedIn: 'root'
})
export class ItemOfTypeService {

  constructor(private http: HttpClient) { }

  getItemsOfType(typeId: number) {
  }
}
