export class ItemSchema {
  id: string;
  name: string;
  properties: ItemSchemaProperty[];
}

export class ItemSchemaProperty {
  name: string;
  type: ItemSchemaPropertyType;
}

export enum ItemSchemaPropertyType {
  Text = 0,
  Number = 1
}
