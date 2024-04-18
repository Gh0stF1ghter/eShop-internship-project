import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catalogEndpoints } from '../constants/environment';
import vendor from '../models/vendorModel';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class VendorService {
  constructor(private http: HttpClient) { }

  getAllVendors() : Observable<vendor[]> {
    return this.http.get<vendor[]>(catalogEndpoints.vendors);
  }

  getVendorById(vendorId: number) : Observable<vendor> {
    return this.http.get<vendor>(`${catalogEndpoints.vendors}/${vendorId}`);
  }
}
