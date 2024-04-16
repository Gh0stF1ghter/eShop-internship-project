import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import typeModel from '../models/typeModel';

@Injectable({
  providedIn: 'root'
})
export class TypeService {
  typesUrl = 'https://localhost:7201/catalog/types'

  constructor(private http: HttpClient) { }

  getTypes() : Observable<typeModel[]> {
    return this.http.get<typeModel[]>(this.typesUrl);
  }

  getTypeById(typeId: number) : Observable<typeModel> {
    return this.http.get<typeModel>(`${this.typesUrl}/${typeId}`);
  }
}