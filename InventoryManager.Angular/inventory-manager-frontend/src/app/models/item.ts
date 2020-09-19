export class Item {

  public constructor(init?: Partial<Item>) {
    Object.assign(this, init);
  }

  id: string;
  name: string;
  type: string;
  quantity: number;
  extraProperties;
}
