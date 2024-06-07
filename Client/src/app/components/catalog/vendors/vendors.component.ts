import { Component, OnInit } from '@angular/core';
import vendor from '../../../models/vendorModel';
import { VendorService } from '../../../services/vendor.service';

@Component({
  selector: 'app-vendors',
  templateUrl: './vendors.component.html',
  styleUrl: './vendors.component.css'
})
export class VendorsComponent implements OnInit {
  vendors: vendor[] = []

  constructor(private vendorService: VendorService) {}

  ngOnInit(): void {
    this.getVendors()
  }

  getVendors() {
    this.vendorService.getAllVendors().subscribe(vendors => this.vendors = vendors)
  }
}
