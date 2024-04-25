import { Component, OnInit } from '@angular/core';
import userBasket from '../../models/basket/userBasket';
import basketItem from '../../models/basket/basketItemModel';
import { SignalrService } from '../../services/signalr.service';
import { HttpClient } from '@microsoft/signalr';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrl: './basket.component.css',
})
export class BasketComponent implements OnInit {
  userId: string | undefined;

  constructor(
    public signalrService: SignalrService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {

    const routeParams = this.route.params;

    routeParams.subscribe((params) => {
      this.userId = params['userId'];

      if (this.userId) {
        this.signalrService.startConnection(this.userId);
        this.signalrService.addUserBasketListener();
        this.signalrService.addBasketItemsListener();  
      }
    });
  }

  startHttpConnection(userId: string) {
    this.signalrService.getUserBasket(userId)

    this.signalrService.GetBasketItems(userId)
  }
}
