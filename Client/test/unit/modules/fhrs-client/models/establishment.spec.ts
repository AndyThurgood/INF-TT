import Establishment from '../../../../../src/modules/fhrs-client/models/establishment';

describe('the Establishment model', () => {
    let mockEstablishment, testEstablishment;
    beforeEach(() => {
        testEstablishment = { BusinessName: 'Test1', RatingKey: '1234' }
        mockEstablishment = new Establishment(testEstablishment);
    });

    it('constructs a valid Establishment object', () =>{
        expect(mockEstablishment.name).toBeDefined();
        expect(mockEstablishment.ratingKey).toBeDefined();
        expect(mockEstablishment.name).toEqual('Test1');
        expect(mockEstablishment.ratingKey).toEqual('1234');
    });

});