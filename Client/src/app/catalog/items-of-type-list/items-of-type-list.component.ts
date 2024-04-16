import { Component, OnInit } from '@angular/core';
import { TypeService } from '../../services/type.service';
import typeModel from '../../models/typeModel';
import item from '../../models/itemModel';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-items-of-type-list',
  templateUrl: './items-of-type-list.component.html',
  styleUrl: './items-of-type-list.component.css'
})
export class ItemsOfTypeListComponent implements OnInit {
  type: typeModel | undefined;
  itemsOfType: item[] = []

  constructor(
    private typeService: TypeService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    let routeParams = this.route.snapshot.paramMap
    let typeId = Number(routeParams.get('typeId'))

    this.getType(typeId)
    this.getItemsOfType(typeId)
  }

  getType(typeId: number) {
    this.typeService.getTypeById(typeId)
      .subscribe(type => this.type = type)
  }
  getItemsOfType(typeId: number) {
  }
}