import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import brand from '../models/brandModel';
import { catalogEndpoints } from '../constants/environment';

@Injectable({
  providedIn: 'root'
})
export class BrandService {

  constructor(private http: HttpClient) { }

  getAllBrands() {
    return this.http.get<brand[]>(catalogEndpoints.brands);
  }

  getBrandById(brandId: number) {
    return this.http.get<brand>(`${catalogEndpoints.brands}/${brandId}`);
  }
}
