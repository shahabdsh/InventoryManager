import { EntityBase } from "./entity-base";

export class Item extends EntityBase {

  id: string;
  name: string;
  schemaId: string;
  quantity: number;
  properties;

  public constructor(init?: Partial<Item>) {
    super();
    Object.assign(this, init);
  }
}
