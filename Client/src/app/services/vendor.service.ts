import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catalogEndpoints } from '../constants/environment';
import vendor from '../models/vendorModel';
import { Observable, firstValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class VendorService {
  constructor(private http: HttpClient) {}

  getAllVendors(): Observable<vendor[]> {
    return this.http.get<vendor[]>(catalogEndpoints.vendors);
  }

  getVendorById(vendorId: number): Observable<vendor> {
    return this.http.get<vendor>(`${catalogEndpoints.vendors}/${vendorId}`);
  }

  async addVendor(name: string) {
    let type = await firstValueFrom(
      this.http.post<vendor>(catalogEndpoints.vendors, { name })
    ).catch(() => {});

    if (type) {
      return type;
    }

    return null;
  }

  updateVendor(vendorId: number, name: string) {
    return this.http.put(
      `${catalogEndpoints.vendors}/${vendorId}`,
      { name },
      {
        observe: 'response',
      }
    );
  }

  deleteVendor(vendorId: number) {
    return this.http.delete(`${catalogEndpoints.vendors}/${vendorId}`, {
      observe: 'response',
    });
  }
}
