import { Component, OnInit } from '@angular/core';
import vendor from '../../../../../models/vendorModel';
import { VendorService } from '../../../../../services/vendor.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ItemComponent } from '../../../../catalog/item/item.component';
import { ItemOfTypeService } from '../../../../../services/item-of-type.service';
import item from '../../../../../models/itemModel';
import createItem from '../../../../../models/createItemModel';

@Component({
  selector: 'app-admin-item',
  templateUrl: './admin-item.component.html',
  styleUrl: './admin-item.component.css',
})
export class AdminItemComponent implements OnInit {
  item: item | undefined;
  itemId: number | undefined;
  typeId: number | undefined;

  editItem: createItem = {
    name: '',
    brandId: 0,
    vendorId: 0,
    stock: 0,
    price: 0
  };
  inputOpened = false;

  constructor(
    private vendorService: ItemOfTypeService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  //Replace to other lifecycle hook
  ngOnInit(): void {
    const routeParams = this.route.params;

    routeParams.subscribe((params) => {
      this.itemId = params['itemId'];
      this.typeId = params['typeId'];

      console.log('subscribe');

      if (this.itemId && this.typeId) {
        this.getItem(this.typeId, this.itemId);
        this.inputOpened = false;
      }
    });
  }

  getItem(typeId: number, itemId: number) {
    this.vendorService.getItemsOfTypeById(typeId, itemId).subscribe((item) => {
      this.item = item;

      this.editItem.name = item.name;
      this.editItem.stock = item.stock;
      this.editItem.price = item.price;
      this.editItem.brandId = item.brandId;
      this.editItem.vendorId = item.vendorId;
    });
  }

  updateBrand() {
    if (this.itemId && this.typeId && this.editItem) {
      if (this.editItem !== this.item) {
        this.vendorService
          .updateItemOfType(this.typeId, this.itemId, this.editItem)
          .subscribe((response) => {
            if (response.status === 204) {
              console.log('updated');
              this.router.navigate(['../', this.itemId], {
                relativeTo: this.route,
              });
            }
          });
      }
    }
  }

  deleteBrand() {
    if (this.itemId && this.typeId) {
      this.vendorService
        .deleteItemOfType(this.typeId, this.itemId)
        .subscribe((response) => {
          if (response.status === 204) {
            console.log('deleted');
            this.router.navigate(['../'], { relativeTo: this.route });
          }
        });
    }
  }
}
