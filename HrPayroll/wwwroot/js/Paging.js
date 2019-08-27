function PagingModel(itempage, totalitems, currentpage, pagecount,prev,next)
{
    this.itempage = itempage;
    this.totalitems = totalitems;
    this.currentpage = currentpage;
    this.pagecount = pagecount;
    this.prev = prev = Boolean;
    this.next = next = Boolean;
}