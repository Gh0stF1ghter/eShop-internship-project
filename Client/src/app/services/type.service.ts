import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import typeModel from '../models/typeModel';
import { catalogEndpoints } from '../constants/environment';

@Injectable({
  providedIn: 'root'
})
export class TypeService {
  constructor(private http: HttpClient) { }

  getTypes() : Observable<typeModel[]> {
    return this.http.get<typeModel[]>(catalogEndpoints.types);
  }

  getTypeById(typeId: number) : Observable<typeModel> {
    return this.http.get<typeModel>(`${catalogEndpoints.types}/${typeId}`);
  }

  addType(name: string) {
    return this.http.post<typeModel>(catalogEndpoints.types, { name })
  }

  updateType(typeId: number, name: string) {
    return this.http.put(`${catalogEndpoints.types}/${typeId}`, { name })
  }

  deleteType(typeId: number) {
    return this.http.delete(`${catalogEndpoints.types}/${typeId}`)
  }
}