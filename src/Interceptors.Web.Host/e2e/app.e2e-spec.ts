import { InterceptorsTemplatePage } from './app.po';

describe('Interceptors App', function() {
  let page: InterceptorsTemplatePage;

  beforeEach(() => {
    page = new InterceptorsTemplatePage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
