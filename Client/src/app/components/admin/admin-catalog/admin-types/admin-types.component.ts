import { Component, OnInit } from '@angular/core';
import type from '../../../../models/typeModel';
import { BrandService } from '../../../../services/brand.service';
import { VendorService } from '../../../../services/vendor.service';
import { TypeService } from '../../../../services/type.service';

@Component({
  selector: 'app-admin-types',
  templateUrl: './admin-types.component.html',
  styleUrl: './admin-types.component.css'
})
export class AdminTypesComponent implements OnInit {
  types: type[] = []

  constructor(private typeService: TypeService) {}

  ngOnInit(): void {
    this.getTypes()
  }

  getTypes() {
    this.typeService.getTypes().subscribe(types => this.types = types)
  }
}