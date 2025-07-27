export class CustomerModel {
  constructor(
    public id: string,
    public name: string,
    public email: string
  ) { }
}
export class CustomerCreateModel {
  constructor(
    public name: string,
    public email: string
  ) { }
}