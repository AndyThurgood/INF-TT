import Rating from '../../../../../src/modules/fhrs-client/models/rating';

describe('the Rating model', () => {
    let  testRating;
    beforeEach(() => {
        testRating = new Rating('name', 'key', 1)
        testRating.percentage = 55.612487;
    });

    it('constructs a valid Rating object', () =>{
        expect(testRating.name).toBeDefined();
        expect(testRating.ratingKey).toBeDefined();
        expect(testRating.schemeType).toBeDefined();
        expect(testRating.name).toEqual('name');
        expect(testRating.ratingKey).toEqual('key');
        expect(testRating.schemeType).toEqual(1);
    });

    it('generates a valid percentage display value', () =>{
        expect(testRating.percentageDisplay()).toEqual('55.6');
    });
});