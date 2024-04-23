import { Component, OnInit } from '@angular/core';
import type from '../../../../../models/typeModel';
import { TypeService } from '../../../../../services/type.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-admin-type',
  templateUrl: './admin-type.component.html',
  styleUrl: './admin-type.component.css',
})
export class AdminTypeComponent implements OnInit {
  type: type | undefined;
  typeId: number | undefined;

  inputOpened = false;
  editName = '';

  constructor(
    private typeService: TypeService,
    private route: ActivatedRoute,
    private router: Router
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

  updateBrand() {
    if (this.typeId) {
      if (this.editName !== this.type?.name) {
        this.typeService
          .updateType(this.typeId, this.editName)
          .subscribe((response) => {
            if (response.status === 204) {
              console.log('updated');
              this.router.navigate(['../', this.typeId], {
                relativeTo: this.route,
              });
            }
          });
      }
    }
  }

  deleteBrand() {
    if (this.typeId) {
      this.typeService.deleteType(this.typeId).subscribe((response) => {
        if (response.status === 204) {
          console.log('deleted');
          this.router.navigate(['../'], { relativeTo: this.route });
        }
      });
    }
  }
}