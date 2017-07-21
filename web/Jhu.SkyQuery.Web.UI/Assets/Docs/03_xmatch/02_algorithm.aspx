<%@ Page Language="C#" AutoEventWireup="true" Title="Algorithm" %>

<asp:Content ContentPlaceHolderID="Text" runat="server">
    <h1>XMatch algorithm</h1>
    <p>Bayes</p>
    <p>The goal of cross-matching is to find all observations across a set of catalogs that are associated to the same celestial object. To assess the goodness of the match, one can try to calculate the posterior probability of the hypotheses $H$ that all observations that are part of the match are of the same source. To avoid working with complex priors, it is more practical to calculate the Bayes factor of $ H = \frac{P(H|D)}{P(H)} / \frac{P(K|D)}{P(K)} $, where the hypotheses $K$ is the complement of hypothesis $H$, i.e. <em>any</em> of the observations in the match belongs to a different celestial object. After applying Bayes&#39; theorem $P(H|D)=\frac{P(D|H)P(H)}{P(D)}$, one arrives at $B(H,K|D) = \frac{P(D|H)}{P(D|K)}$. </p>
    <p>HTM index</p>
    <p>Zone index</p>
    <p>Prefiltering</p>
    <p>&nbsp;</p>
</asp:Content>
