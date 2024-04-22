import { Component, OnInit } from '@angular/core';
import vendor from '../../../../../models/vendorModel';
import { VendorService } from '../../../../../services/vendor.service';
import { ActivatedRoute } from '@angular/router';
import { ItemComponent } from '../../../../catalog/item/item.component';
import { ItemOfTypeService } from '../../../../../services/item-of-type.service';
import item from '../../../../../models/itemModel';

@Component({
  selector: 'app-admin-item',
  templateUrl: './admin-item.component.html',
  styleUrl: './admin-item.component.css'
})
export class AdminItemComponent implements OnInit {
  item: item | undefined;
  itemId: number | undefined;
  typeId: number | undefined;

  constructor(
    private vendorService: ItemOfTypeService,
    private route: ActivatedRoute
  ) {}

  //Replace to other lifecycle hook
  ngOnInit(): void {
    const routeParams = this.route.params;

    routeParams.subscribe((params) => {
      this.itemId = params['itemId'];
      this.typeId = params['typeId']

      console.log('subscribe');

      if (this.itemId && this.typeId) {
        this.getItem(this.typeId, this.itemId);
      }
    });
  }

  getItem(typeId: number, itemId: number) {
    this.vendorService
      .getItemsOfTypeById(typeId, itemId)
      .subscribe((item) => (this.item = item));
  }
}