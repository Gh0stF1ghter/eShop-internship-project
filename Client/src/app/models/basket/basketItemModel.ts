import ItemModel from "./ItembasketReferenceModel";

export default interface basketItem {
  basketItemId: string;
  quantity: number;
  sumPrice: number;
  item: ItemModel;
}
