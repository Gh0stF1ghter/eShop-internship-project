import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import typeModel from '../models/typeModel';
import { endpoints } from '../constants/environment';

@Injectable({
  providedIn: 'root'
})
export class TypeService {
  constructor(private http: HttpClient) { }

  getTypes() : Observable<typeModel[]> {
    return this.http.get<typeModel[]>(endpoints.types);
  }

  getTypeById(typeId: number) : Observable<typeModel> {
    return this.http.get<typeModel>(`${endpoints.types}/${typeId}`);
  }
}