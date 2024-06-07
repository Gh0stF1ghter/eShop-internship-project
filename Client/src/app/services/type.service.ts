import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, firstValueFrom } from 'rxjs';
import typeModel from '../models/typeModel';
import { catalogEndpoints } from '../constants/environment';

@Injectable({
  providedIn: 'root',
})
export class TypeService {
  constructor(private http: HttpClient) {}

  getTypes(): Observable<typeModel[]> {
    return this.http.get<typeModel[]>(catalogEndpoints.types);
  }

  getTypeById(typeId: number): Observable<typeModel> {
    return this.http.get<typeModel>(`${catalogEndpoints.types}/${typeId}`);
  }

  async addType(name: string) {
    let type = await firstValueFrom(
      this.http.post<typeModel>(catalogEndpoints.types, { name })
    ).catch(() => {});

    if (type) {
      return type;
    }

    return null;
  }

  updateType(typeId: number, name: string) {
    return this.http.put(`${catalogEndpoints.types}/${typeId}`, { name }, {
      observe: 'response',
    });
  }

  deleteType(typeId: number) {
    return this.http.delete(`${catalogEndpoints.types}/${typeId}`, {
      observe: 'response',
    });
  }
}
