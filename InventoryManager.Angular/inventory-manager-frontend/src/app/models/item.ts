export class Item {

  id: string;
  name: string;
  type: string;
  quantity: number;
  properties;

  public constructor(init?: Partial<Item>) {
    Object.assign(this, init);
  }
}
