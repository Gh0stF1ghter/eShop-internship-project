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

  addBrand(name: string) {
    return this.http.post<brand>(catalogEndpoints.brands, { name })
  }

  updateBrand(brandId: number, name: string) {
    return this.http.put(`${catalogEndpoints.brands}/${brandId}`, { name })
  }

  deleteBrand(brandId: number) {
    return this.http.delete(`${catalogEndpoints.brands}/${brandId}`)
  }
}
