export class CategoryDTO {
    constructor(
        public id: string,
        public name: string,
        public description: string
    ) { }
}
export class CategoryCreateDTO {
    constructor(
        public name: string,
        public description: string
    ) { }
}