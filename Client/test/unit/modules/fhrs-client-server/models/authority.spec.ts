import Authority from '../../../../../src/modules/fhrs-client-server/models/authority';

describe('the Authority model', () => {
    let mockAuthority, testAuthority;
    beforeEach(() => {
        testAuthority = { Name: 'Test1', LocalAuthorityId: '1234', SchemeType: '1' }
        mockAuthority = new Authority(testAuthority);
    });

    it('constructs a valid Authority object', () =>{
        expect(mockAuthority.name).toBeDefined();
        expect(mockAuthority.localAuthorityId).toBeDefined();
        expect(mockAuthority.schemeType).toBeDefined();
        expect(mockAuthority.name).toEqual('Test1');
        expect(mockAuthority.localAuthorityId).toEqual(1234);
        expect(mockAuthority.schemeType).toEqual(1);
    });

});