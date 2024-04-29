import { Injectable } from '@angular/core';
import basketItem from '../models/basket/basketItemModel';
import * as signalR from '@microsoft/signalr';
import { gateway, hub } from '../constants/environment';
import userBasket from '../models/basket/userBasket';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class SignalrService {
  public basketItems: basketItem[] = [];
  public userBasket: userBasket | undefined;

  private hubConnection: signalR.HubConnection | undefined;
  public startConnection = (userId: string) => {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(hub)
      .build();

    this.hubConnection
      .start()
      .then(() => {
        console.log('signalR connection established');

        this.hubConnection?.send('getUserBasket', userId);
        this.hubConnection?.send('getBasketItems', userId);
      })
      .catch((err) =>
        console.log('Error while establishing connection: ' + err)
      );
  };

  public addBasketItemsListener = () => {
    if (this.hubConnection)
      this.hubConnection.on('basketItemsReceived', (basketItems) => {
        this.basketItems = basketItems;
        console.log(basketItems);
      });
  };

  public addUserBasketListener = () => {
    if (this.hubConnection)
      this.hubConnection.on('userBasketReceived', (userbasket) => {
        this.userBasket = userbasket;
      });
  };

  public async updateBasketItemQuantity(basketItem: basketItem, userId: string, quantity: number) {
    await this.hubConnection?.invoke('updateBasketItemQuantity', userId, basketItem.basketItemId, quantity)
  }

  public async deleteBasketItem(userId: string, basketItemId: string) {
    await this.hubConnection?.invoke('deleteBasketItem', userId, basketItemId)
  }
}