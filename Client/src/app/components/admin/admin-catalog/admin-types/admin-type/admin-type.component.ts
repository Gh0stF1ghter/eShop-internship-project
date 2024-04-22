import { Component, OnInit } from '@angular/core';
import type from '../../../../../models/typeModel';
import { TypeService } from '../../../../../services/type.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-admin-type',
  templateUrl: './admin-type.component.html',
  styleUrl: './admin-type.component.css'
})
export class AdminTypeComponent implements OnInit {
  type: type | undefined;
  typeId: number | undefined;

  constructor(
    private typeService: TypeService,
    private route: ActivatedRoute
  ) {}

  //Replace to other lifecycle hook
  ngOnInit(): void {
    const routeParams = this.route.params;

    routeParams.subscribe((params) => {
      this.typeId = params['typeId'];

      console.log('subscribe');

      if (this.typeId) {
        this.getType(this.typeId);
      }
    });
  }

  getType(typeId: number) {
    this.typeService
      .getTypeById(typeId)
      .subscribe((vendor) => (this.type = vendor));
  }
}
