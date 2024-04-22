import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import brand from '../models/brandModel';
import { catalogEndpoints } from '../constants/environment';
import { firstValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class BrandService {
  constructor(private http: HttpClient) {}

  getAllBrands() {
    return this.http.get<brand[]>(catalogEndpoints.brands);
  }

  getBrandById(brandId: number) {
    return this.http.get<brand>(`${catalogEndpoints.brands}/${brandId}`);
  }

  async addBrand(name: string) {
    let brand = await firstValueFrom(
      this.http.post<brand>(catalogEndpoints.brands, { name })
    ).catch(() => {});

    if (brand) {
      return brand;
    }

    return false;
  }

  updateBrand(brandId: number, name: string) {
    return this.http.put(`${catalogEndpoints.brands}/${brandId}`, { name });
  }

  deleteBrand(brandId: number) {
    return this.http.delete(`${catalogEndpoints.brands}/${brandId}`);
  }
}
